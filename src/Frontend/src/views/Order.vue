<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="orderProducts"
      :search="search"
      sort-by="id"
      class="elevation-1"
    >
      <template v-slot:item.quantity="props">
        <v-edit-dialog :return-value.sync="props.item.quantity" persistent>
          {{ props.item.quantity }}
          <template v-slot:input>
            <v-text-field
              v-model="props.item.quantity"
              type="number"
              label="Quantity"
              single-line
            ></v-text-field>
          </template>
        </v-edit-dialog>
      </template>
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Order product</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            label="Search"
            single-line
            hide-details
          ></v-text-field>
          <v-spacer></v-spacer>
          <v-select
            :items="customers"
            v-model="customer"
            name="firstName"
            :item-text="customerName"
            item-value="id"
            return-object
            label="Select customer"
            solo
            dense
            flat
            single-line
            hide-details
          ></v-select>
          <v-dialog v-model="confirmationDialog" max-width="500px">
            <v-card>
              <v-card-title class="text-h5"
                >Are you sure you want to order this product?</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeOrderProduct"
                  >Cancel</v-btn
                >
                <v-btn color="blue darken-1" text @click="orderProductConfirm"
                  >OK</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.buy="{ item }">
        <v-icon small @click="orderProduct(item)"> mdi-cart-plus </v-icon>
      </template>
    </v-data-table>

    <v-snackbar v-model="snack" :timeout="3000" :color="snackColor">
      {{ snackText }}

      <template v-slot:action="{ attrs }">
        <v-btn v-bind="attrs" text @click="snack = false"> Close </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script lang="ts">
import { ICustomer } from "@/@types/customer";
import { ICreateOrder } from "@/@types/order";
import useCustomer from "@/composables/useCustomer";
import useOrder from "@/composables/useOrder";
import {
  defineComponent,
  nextTick,
  onBeforeMount,
  ref,
  watch,
} from "@vue/composition-api";
import { IOrderProduct } from "@/@types/orderProduct";

export default defineComponent({
  name: "Order",
  setup() {
    const { customers, getAll: getAllCustomers } = useCustomer();
    const {
      orderProducts,
      defaultProduct,
      orderedProduct,
      setProducts,
      createOrder,
    } = useOrder();

    const snack = ref(false);
    const snackColor = ref("");
    const snackText = ref("");
    const confirmationDialog = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "Product name", value: "productName" },
      { text: "Price", value: "price" },
      { text: "Quantity", value: "quantity" },
      { text: "Buy", value: "buy", sortable: false },
    ]);
    const customer = ref<ICustomer>({ id: 0, firstName: "", lastName: "" });
    const orderedIndex = ref(-1);

    const openSnack = (txt: string, color: string) => {
      snack.value = true;
      snackColor.value = color;
      snackText.value = txt;
    };

    const customerName = (customer: ICustomer) => {
      return `${customer.firstName} ${customer.lastName}`;
    };

    const orderProduct = (product: IOrderProduct) => {
      orderedIndex.value = orderProducts.value.indexOf(product);
      orderedProduct.value = product;
      confirmationDialog.value = true;
    };

    const closeOrderProduct = () => {
      confirmationDialog.value = false;
      nextTick(() => {
        orderedProduct.value = defaultProduct.value;
        orderedIndex.value = -1;
      });
    };

    const orderProductConfirm = async () => {
      if (
        !customer.value ||
        customer.value.id === 0 ||
        orderedProduct.value.quantity < 1
      ) {
        openSnack(
          "you have to select a customer and order at least 1 product",
          "warning"
        );
        return;
      }

      try {
        const body: ICreateOrder = {
          customerId: customer.value.id,
          productId: orderedProduct.value.id,
          quantity: orderedProduct.value.quantity,
        };
        const response = await createOrder(body);

        if (response.ok) {
          closeOrderProduct();
          openSnack(
            `${customer.value.firstName} ${customer.value.lastName} ordered ${orderedProduct.value.quantity} ${orderedProduct.value.productName}`,
            "success"
          );
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }
    };

    onBeforeMount(async () => {
      await setProducts().catch((error) => openSnack(error, "error"));
    });

    watch(confirmationDialog, (val) => {
      val || closeOrderProduct();
    });

    onBeforeMount(
      async () =>
        await getAllCustomers().catch((error) => openSnack(error, "error"))
    );

    return {
      snack,
      snackText,
      snackColor,
      confirmationDialog,
      search,
      headers,
      customer,
      customers,
      customerName,
      orderProducts,
      orderProduct,
      orderProductConfirm,
      closeOrderProduct,
    };
  },
});
</script>
