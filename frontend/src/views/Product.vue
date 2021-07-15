<template>
  <v-data-table
    :headers="headers"
    :items="products"
    :search="search"
    sort-by="id"
    class="elevation-1"
  >
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Product</v-toolbar-title>
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
        <v-dialog v-model="dialog" max-width="500px">
          <template v-slot:activator="{ on, attrs }">
            <v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on">
              New Product
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      v-model="editedProduct.id"
                      readonly
                      label="Id"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      v-model="editedProduct.product_name"
                      label="Product name"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      v-model="editedProduct.price"
                      label="Price"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="close"> Cancel </v-btn>
              <v-btn color="blue darken-1" text @click="save"> Save </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5"
              >Are you sure you want to delete this customer?</v-card-title
            >
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDelete"
                >Cancel</v-btn
              >
              <v-btn color="blue darken-1" text @click="deleteProductConfirm"
                >OK</v-btn
              >
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item.actions="{ item }">
      <v-icon small class="mr-2" @click="editProduct(item)">
        mdi-pencil
      </v-icon>
      <v-icon small @click="deleteProduct(item)"> mdi-delete </v-icon>
    </template>
    <template v-slot:no-data>
      <v-btn color="primary" @click="mock"> Reset </v-btn>
    </template>
  </v-data-table>
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
import { IProduct } from "../@types/product";

export default defineComponent({
  setup() {
    const dialog = ref(false);
    const dialogDelete = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "Product name", value: "product_name" },
      { text: "Price", value: "price" },
      { text: "Actions", value: "actions", sortable: false },
    ]);
    const products = ref<IProduct[]>([]);
    const editedIndex = ref(-1);
    const editedProduct = ref<IProduct>({
      id: 0,
      product_name: "",
      price: 0,
    });
    const defaultCustomer = ref<IProduct>({
      id: 0,
      product_name: "",
      price: 0,
    });

    const formTitle = computed(() =>
      editedIndex.value === -1 ? "New Product" : "Edit Product"
    );

    const mock = () => {
      products.value = [
        {
          id: 1,
          product_name: "product 1",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 2,
          product_name: "product 2",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 3,
          product_name: "product 3",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 4,
          product_name: "product 4",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 5,
          product_name: "product 5",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 6,
          product_name: "product 6",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 7,
          product_name: "product 7",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 8,
          product_name: "product 8",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 9,
          product_name: "product 9",
          price: Math.floor(Math.random() * 100),
        },
        {
          id: 10,
          product_name: "product 10",
          price: Math.floor(Math.random() * 100),
        },
      ];
    };

    const editProduct = (product: IProduct) => {
      editedIndex.value = products.value.indexOf(product);
      editedProduct.value = Object.assign({}, product);
      dialog.value = true;
    };

    const deleteProduct = (product: IProduct) => {
      editedIndex.value = products.value.indexOf(product);
      editedProduct.value = Object.assign({}, product);
      dialogDelete.value = true;
    };

    const deleteProductConfirm = () => {
      products.value.splice(editedIndex.value, 1);
      closeDelete();
    };

    const close = () => {
      dialog.value = false;
      nextTick(() => {
        editedProduct.value = Object.assign({}, defaultCustomer.value);
        editedIndex.value = -1;
      });
    };

    const closeDelete = () => {
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
      val || closeDelete();
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
      formTitle,
      mock,
      editProduct,
      deleteProduct,
      deleteProductConfirm,
      close,
      closeDelete,
      save,
    };
  },
});
</script>
