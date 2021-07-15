<template>
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
    <template v-slot:no-data>
      <v-btn color="primary" @click="mock"> Reset </v-btn>
    </template>
  </v-data-table>
</template>

<script lang="ts">
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
    const dialog = ref(false);
    const dialogDelete = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "Product name", value: "product_name" },
      { text: "Price", value: "price" },
      { text: "Quantity", value: "quantity" },
      { text: "Buy", value: "buy", sortable: false },
    ]);
    const products = ref<IOrderProduct[]>([]);
    const editedIndex = ref(-1);
    const editedProduct = ref<IOrderProduct>({
      id: 0,
      product_name: "",
      price: 0,
      quantity: 0,
    });
    const defaultCustomer = ref<IOrderProduct>({
      id: 0,
      product_name: "",
      price: 0,
      quantity: 0,
    });

    const mock = () => {
      products.value = [
        {
          id: 1,
          product_name: "product 1",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 2,
          product_name: "product 2",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 3,
          product_name: "product 3",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 4,
          product_name: "product 4",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 5,
          product_name: "product 5",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 6,
          product_name: "product 6",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 7,
          product_name: "product 7",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 8,
          product_name: "product 8",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 9,
          product_name: "product 9",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
        {
          id: 10,
          product_name: "product 10",
          price: Math.floor(Math.random() * 100),
          quantity: 0,
        },
      ];
    };

    const orderProduct = (product: IOrderProduct) => {
      editedIndex.value = products.value.indexOf(product);
      editedProduct.value = Object.assign({}, product);
      dialogDelete.value = true;
    };

    const orderProductConfirm = () => {
      products.value.splice(editedIndex.value, 1);
      closeOrderProduct();
    };

    const close = () => {
      dialog.value = false;
      nextTick(() => {
        editedProduct.value = Object.assign({}, defaultCustomer.value);
        editedIndex.value = -1;
      });
    };

    const closeOrderProduct = () => {
      dialogDelete.value = false;
      nextTick(() => {
        editedProduct.value = Object.assign({}, defaultCustomer.value);
        editedIndex.value = -1;
      });
    };

    const save = () => {
      if (editedIndex.value > -1) {
        Object.assign(products.value[editedIndex.value], editedProduct.value);
      } else {
        products.value.push(editedProduct.value);
      }
      close();
    };

    onBeforeMount(mock);

    watch(dialog, (val) => {
      val || close();
    });

    watch(dialogDelete, (val) => {
      val || closeOrderProduct();
    });

    return {
      dialog,
      dialogDelete,
      search,
      headers,
      products,
      editedIndex,
      editedProduct,
      defaultCustomer,
      mock,
      orderProduct,
      orderProductConfirm,
      close,
      closeOrderProduct,
      save,
    };
  },
});
</script>
