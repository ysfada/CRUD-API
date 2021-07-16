<template>
  <div>
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
                        v-model="editedProduct.productName"
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
                <v-btn color="blue darken-1" text @click="close">
                  Cancel
                </v-btn>
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
import useProduct from "@/composables/useProduct";
import {
  computed,
  defineComponent,
  nextTick,
  onBeforeMount,
  ref,
  watch,
} from "@vue/composition-api";
import { ICreateProduct, IProduct, IUpdateProduct } from "../@types/product";

export default defineComponent({
  setup() {
    const { deleteProductById, updateProduct, createProduct, getAllProducts } =
      useProduct();
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
      { text: "Actions", value: "actions", sortable: false },
    ]);
    const products = ref<IProduct[]>([]);
    const editedIndex = ref(-1);
    const editedProduct = ref<IProduct>({
      id: 0,
      productName: "",
      price: 0,
    });
    const defaultCustomer = ref<IProduct>({
      id: 0,
      productName: "",
      price: 0,
    });

    const formTitle = computed(() =>
      editedIndex.value === -1 ? "New Product" : "Edit Product"
    );

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

    const deleteProductConfirm = async () => {
      try {
        const response = await deleteProductById(editedProduct.value.id);
        if (response.ok) {
          products.value.splice(editedIndex.value, 1);
          closeDelete();
          showSnack("Product deleted", "info");
        } else {
          // TODO: handle 4xx, 3xx
        }
      } catch (error) {
        showSnack(error, "error");
      }

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

    const save = async () => {
      // TODO validate data
      if (
        editedProduct.value.productName.length < 1 ||
        editedProduct.value.price < 0
      )
        return;

      if (editedIndex.value > -1) {
        try {
          const body: IUpdateProduct = {
            productName: editedProduct.value.productName,
            price: editedProduct.value.price,
          };
          const response = await updateProduct(editedProduct.value.id, body);

          if (response.ok) {
            Object.assign(
              products.value[editedIndex.value],
              editedProduct.value
            );
            dialog.value = true;
            showSnack("Product updated", "info");
          } else {
            // TODO: handle 4xx, 3xx
          }
        } catch (error) {
          showSnack(error, "error");
        }
      } else {
        try {
          const body: ICreateProduct = {
            productName: editedProduct.value.productName,
            price: editedProduct.value.price,
          };
          const response = await createProduct(body);
          if (response.ok) {
            products.value.push(await response.json());
            showSnack("Product created", "info");
          } else {
            // TODO: handle 4xx, 3xx
          }
        } catch (error) {
          showSnack(error, "error");
        }
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
      val || closeDelete();
    });

    return {
      snack,
      snackText,
      snackColor,
      dialog,
      dialogDelete,
      search,
      headers,
      products,
      editedIndex,
      editedProduct,
      defaultCustomer,
      formTitle,
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
