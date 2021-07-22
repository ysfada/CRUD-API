<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="orders"
      :search="search"
      sort-by="id"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Orders</v-toolbar-title>
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
          <v-dialog
            v-model="formDialog"
            max-width="500px"
            :fullscreen="isNewOrder"
            @keydown.esc="closeFormDialog"
            @click:outside="closeFormDialog"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                color="primary"
                dark
                class="mb-2"
                v-bind="attrs"
                v-on="on"
                @click="isNewOrder = true"
              >
                New Order
              </v-btn>
            </template>
            <v-form ref="formRef" v-model="valid" lazy-validation>
              <v-card>
                <v-card-title>
                  <span class="text-h5">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                  <v-container v-if="isNewOrder">
                    <v-data-table
                      :headers="orderHeaders"
                      :items="orderProducts"
                      :search="search"
                      sort-by="id"
                      class="elevation-1"
                    >
                      <template v-slot:item.quantity="props">
                        <v-edit-dialog
                          :return-value.sync="props.item.quantity"
                          persistent
                        >
                          {{ props.item.quantity }}
                          <template v-slot:input>
                            <v-text-field
                              v-model="props.item.quantity"
                              type="number"
                              label="Quantity"
                              single-line
                              :rules="rules"
                              required
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
                            required
                            label="Select customer"
                            solo
                            dense
                            flat
                            single-line
                            hide-details
                          ></v-select>
                          <v-dialog
                            v-model="confirmationDialog"
                            max-width="500px"
                            :retain-focus="false"
                          >
                            <v-card>
                              <v-card-title class="text-h5"
                                >Are you sure you want to order this
                                product?</v-card-title
                              >
                              <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn
                                  color="blue darken-1"
                                  text
                                  @click="closeOrderProduct"
                                  >Cancel</v-btn
                                >
                                <v-btn
                                  color="blue darken-1"
                                  text
                                  :disabled="!valid"
                                  @click="orderProductConfirm"
                                  >OK</v-btn
                                >
                                <v-spacer></v-spacer>
                              </v-card-actions>
                            </v-card>
                          </v-dialog>
                        </v-toolbar>
                      </template>
                      <template v-slot:item.buy="{ item }">
                        <v-icon small @click="orderProduct(item)">
                          mdi-cart-plus
                        </v-icon>
                      </template>
                    </v-data-table>
                  </v-container>
                  <v-container v-else>
                    <v-row>
                      <v-col cols="12" sm="6">
                        <v-text-field
                          v-model="editedOrder.id"
                          readonly
                          required
                          label="Id"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="6">
                        <v-text-field
                          v-model="editedOrder.productId"
                          label="Product id"
                          :rules="rules"
                          required
                        ></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="6">
                        <v-text-field
                          v-model="editedOrder.customerId"
                          label="Customer id"
                          :rules="rules"
                          required
                        ></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="6">
                        <v-text-field
                          v-model="editedOrder.quantity"
                          label="Quantity"
                          :rules="rules"
                          required
                        ></v-text-field>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="closeFormDialog">
                    Cancel
                  </v-btn>
                  <v-btn
                    v-show="!isNewOrder"
                    color="blue darken-1"
                    text
                    :disabled="!valid"
                    @click="save"
                  >
                    Save
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-form>
          </v-dialog>
          <v-dialog v-model="confirmationDialog" max-width="500px">
            <v-card>
              <v-card-title class="text-h5"
                >Are you sure you want to delete this order?</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                  color="blue darken-1"
                  text
                  @click="confirmationDialogCancel"
                  >Cancel</v-btn
                >
                <v-btn
                  color="blue darken-1"
                  text
                  :disabled="!valid"
                  @click="confirmationDialogOk"
                  >OK</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon small class="mr-2" @click="edit(item)"> mdi-pencil </v-icon>
        <v-icon small @click="remove(item)"> mdi-delete </v-icon>
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
import {
  computed,
  defineComponent,
  nextTick,
  onBeforeMount,
  ref,
  watch,
} from "@vue/composition-api";
import useOrder from "@/composables/useOrder";
import { ICreateOrder, IOrder, IUpdateOrder } from "@/@types/order";
import useCustomer from "../composables/useCustomer";
import { ICustomer } from "../@types/customer";
import { IOrderProduct } from "../@types/orderProduct";

export default defineComponent({
  name: "Order",
  setup() {
    const {
      orders,
      orderProducts,
      defaultOrder,
      editedOrder,
      defaultProduct,
      editedProduct,
      getAll,
      removeById,
      updateById,
      create,
      setProducts,
    } = useOrder();
    const { customers, getAll: getAllCustomers } = useCustomer();

    const rules = ref([
      (value: number) => !!value || "Required.",
      (value: number) => (value || 0) > 0 || "Min 1",
    ]);

    const formRef = ref<HTMLFormElement>();
    const valid = ref(true);
    const isNewOrder = ref(true);
    const snack = ref(false);
    const snackColor = ref("");
    const snackText = ref("");
    const formDialog = ref(false);
    const confirmationDialog = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "Product id", value: "productId" },
      { text: "Product name", value: "product.productName" },
      { text: "Customer id", value: "customerId" },
      { text: "Customer firstname", value: "customer.firstName" },
      { text: "Customer lastname", value: "customer.lastName" },
      { text: "Quantity", value: "quantity" },
      { text: "Actions", value: "actions", sortable: false },
    ]);
    const editedIndex = ref(-1);
    const formTitle = computed(() =>
      isNewOrder.value ? "New Order" : "Edit Order"
    );

    const orderHeaders = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "Product name", value: "productName" },
      { text: "Price", value: "price" },
      { text: "Quantity", value: "quantity" },
      { text: "Buy", value: "buy", sortable: false },
    ]);
    const orderedIndex = ref(-1);
    const customer = ref<ICustomer>({ id: 0, firstName: "", lastName: "" });
    const customerName = (customer: ICustomer) => {
      return `${customer.firstName} ${customer.lastName}`;
    };

    const openSnack = (txt: string, color: string) => {
      snack.value = true;
      snackColor.value = color;
      snackText.value = txt;
    };

    const edit = (order: IOrder) => {
      isNewOrder.value = false;
      editedIndex.value = orders.value.indexOf(order);
      editedOrder.value = order;
      formDialog.value = true;
    };

    const remove = (order: IOrder) => {
      isNewOrder.value = false;
      editedIndex.value = orders.value.indexOf(order);
      editedOrder.value = order;
      confirmationDialog.value = true;
    };

    const closeFormDialog = async () => {
      await getAll().catch((error) => openSnack(error, "error"));
      formDialog.value = false;
      formRef.value?.resetValidation();
      nextTick(() => {
        editedOrder.value = defaultOrder.value;
        editedIndex.value = -1;
      });
    };

    const confirmationDialogCancel = () => {
      confirmationDialog.value = false;
      nextTick(() => {
        editedOrder.value = defaultOrder.value;
        editedIndex.value = -1;
      });
    };

    const confirmationDialogOk = async () => {
      try {
        const response = await removeById(editedOrder.value.id);
        if (response.ok) {
          openSnack("Order deleted", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }

      confirmationDialogCancel();
    };

    const save = async () => {
      if (!formRef.value || !formRef.value?.validate()) return;

      if (editedIndex.value > -1) {
        await updateOrder();
      } else {
        await createOrder();
      }

      closeFormDialog();
    };

    const updateOrder = async () => {
      try {
        const payload: IUpdateOrder = {
          customerId: editedOrder.value.customerId,
          productId: editedOrder.value.productId,
          quantity: editedOrder.value.quantity,
        };
        const response = await updateById(editedOrder.value.id, payload);

        if (response.ok) {
          openSnack("Order updated", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }
    };

    const createOrder = async () => {
      try {
        const payload: ICreateOrder = {
          customerId: editedOrder.value.customerId,
          productId: editedOrder.value.productId,
          quantity: editedOrder.value.quantity,
        };
        const response = await create(payload);
        if (response.ok) {
          openSnack("Order created", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }

      closeOrderProduct();
    };

    const orderProduct = (product: IOrderProduct) => {
      orderedIndex.value = orderProducts.value.indexOf(product);
      editedProduct.value = product;
      editedOrder.value = {
        id: product.id,
        customerId: customer.value.id,
        productId: product.id,
        quantity: product.quantity,
      };
      confirmationDialog.value = true;
    };

    const closeOrderProduct = () => {
      confirmationDialog.value = false;
      nextTick(() => {
        editedProduct.value = defaultProduct.value;
        orderedIndex.value = -1;
      });
    };

    const orderProductConfirm = async () => {
      if (
        !customer.value ||
        customer.value.id === 0 ||
        editedProduct.value.quantity < 1
      ) {
        openSnack(
          "you have to select a customer and quantity cannot be 0 or less",
          "warning"
        );
        closeOrderProduct();
        return;
      }

      await createOrder();
    };

    // watch(formDialog, (val) => {
    //   val || closeFormDialog();
    // });

    watch(confirmationDialog, (val) => {
      val || confirmationDialogCancel();
    });

    onBeforeMount(
      async () => await getAll().catch((error) => openSnack(error, "error"))
    );

    onBeforeMount(
      async () =>
        await getAllCustomers().catch((error) => openSnack(error, "error"))
    );
    onBeforeMount(
      async () =>
        await setProducts().catch((error) => openSnack(error, "error"))
    );

    return {
      // formRef,
      valid,
      isNewOrder,
      snack,
      snackText,
      snackColor,
      formDialog,
      confirmationDialog,
      search,
      headers,
      orders,
      editedOrder,
      formTitle,
      rules,
      edit,
      remove,
      confirmationDialogOk,
      closeFormDialog,
      confirmationDialogCancel,
      save,
      orderHeaders,
      orderProducts,
      customers,
      customer,
      customerName,
      orderProduct,
      closeOrderProduct,
      orderProductConfirm,
    };
  },
});
</script>
