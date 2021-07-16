<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="products"
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
            :item-text="
              (customer) => `${customer.firstName} ${customer.lastName}`
            "
            item-value="id"
            return-object
            label="Select customer"
            solo
            dense
            flat
            single-line
            hide-details
          ></v-select>
          <v-dialog v-model="dialogDelete" max-width="500px">
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
import useProduct from "@/composables/useProduct";
import {
  defineComponent,
  nextTick,
  onBeforeMount,
  ref,
  watch,
} from "@vue/composition-api";
import { IOrderProduct } from "../@types/orderProduct";

export default defineComponent({
  setup() {
    const { getAllCustomers } = useCustomer();
    const { getAllProducts } = useProduct();
    const { createOrder } = useOrder();
    const snack = ref(false);
    const snackColor = ref("");
    const snackText = ref("");
    const dialog = ref(false);
    const dialogDelete = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "Product name", value: "productName" },
      { text: "Price", value: "price" },
      { text: "Quantity", value: "quantity" },
      { text: "Buy", value: "buy", sortable: false },
    ]);
    const customers = ref<ICustomer[]>([]);
    const customer = ref<ICustomer>({ id: 0, firstName: "", lastName: "" });
    const products = ref<IOrderProduct[]>([]);
    const orderedIndex = ref(-1);
    const orderedProduct = ref<IOrderProduct>({
      id: 0,
      productName: "",
      price: 0,
      quantity: 0,
    });
    const defaultProduct = ref<IOrderProduct>({
      id: 0,
      productName: "",
      price: 0,
      quantity: 0,
    });

    const orderProduct = (product: IOrderProduct) => {
      orderedIndex.value = products.value.indexOf(product);
      orderedProduct.value = Object.assign({}, product);
      dialogDelete.value = true;
    };

    const orderProductConfirm = async () => {
      // TODO validate data
      if (
        !customer.value ||
        customer.value.id === 0 ||
        orderedProduct.value.quantity < 1
      )
        return;

      try {
        const body: ICreateOrder = {
          customerId: customer.value.id,
          productId: orderedProduct.value.id,
          quantity: orderedProduct.value.quantity,
        };
        const response = await createOrder(body);

        if (response.ok) {
          closeOrderProduct();
          showSnack(
            `${customer.value.firstName} ${customer.value.lastName} bought ${orderedProduct.value.productName} quantity: ${orderedProduct.value.quantity}`,
            "info"
          );
        } else {
          // TODO: handle 4xx, 3xx
        }
      } catch (error) {
        showSnack(error, "error");
      }
    };

    const close = () => {
      dialog.value = false;
      nextTick(() => {
        orderedProduct.value = Object.assign({}, defaultProduct.value);
        orderedIndex.value = -1;
      });
    };

    const closeOrderProduct = () => {
      dialogDelete.value = false;
      nextTick(() => {
        orderedProduct.value = Object.assign({}, defaultProduct.value);
        orderedIndex.value = -1;
      });
    };

    const save = () => {
      if (orderedIndex.value > -1) {
        Object.assign(products.value[orderedIndex.value], orderedProduct.value);
      } else {
        products.value.push(orderedProduct.value);
      }
      close();
    };

    const getProducts = async () => {
      try {
        const response = await getAllProducts();
        if (response.ok) {
          products.value = await response.json();
        } else {
          // TODO: handle 4xx, 3xx
        }
      } catch (error) {
        showSnack(error, "error");
      }
    };

    const getCustomers = async () => {
      try {
        const response = await getAllCustomers();
        if (response.ok) {
          customers.value = await response.json();
        } else {
          // TODO: handle 4xx, 3xx
        }
      } catch (error) {
        showSnack(error, "error");
      }
    };

    onBeforeMount(getCustomers);

    onBeforeMount(getProducts);

    const showSnack = (txt: string, color: string) => {
      snack.value = true;
      snackColor.value = color;
      snackText.value = txt;
    };

    watch(dialog, (val) => {
      val || close();
    });

    watch(dialogDelete, (val) => {
      val || closeOrderProduct();
    });

    return {
      snack,
      snackText,
      snackColor,
      dialog,
      dialogDelete,
      search,
      headers,
      customer,
      customers,
      products,
      orderedIndex,
      orderedProduct,
      defaultProduct,
      orderProduct,
      orderProductConfirm,
      close,
      closeOrderProduct,
      save,
    };
  },
});
</script>
