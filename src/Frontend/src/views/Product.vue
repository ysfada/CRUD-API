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
          <v-toolbar-title>Products</v-toolbar-title>
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
          <v-dialog v-model="formDialog" max-width="500px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                color="primary"
                dark class="mb-2"
                v-bind="attrs"
                v-on="on"
                @click="isNewProduct = true"
              >
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
                    <v-col v-show="!isNewProduct" cols="12" sm="6">
                      <v-text-field
                        v-model="editedProduct.id"
                        readonly
                        label="Id"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6">
                      <v-text-field
                        v-model="editedProduct.productName"
                        label="Product name"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6">
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
                <v-btn color="blue darken-1" text @click="closeFormDialog">
                  Cancel
                </v-btn>
                <v-btn color="blue darken-1" text @click="save"> Save </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
          <v-dialog v-model="confirmationDialog" max-width="500px">
            <v-card>
              <v-card-title class="text-h5"
                >Are you sure you want to delete this product?</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                  color="blue darken-1"
                  text
                  @click="confirmationDialogCancel"
                  >Cancel</v-btn
                >
                <v-btn color="blue darken-1" text @click="confirmationDialogOk"
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
import { ICreateProduct, IProduct, IUpdateProduct } from "@/@types/product";
import useProduct from "@/composables/useProduct";

export default defineComponent({
  name: "Product",
  setup() {
    const {
      products,
      defaultProduct,
      editedProduct,
      getAll,
      removeById,
      updateById,
      create,
    } = useProduct();

    const isNewProduct = ref(true);
    const snack = ref(false);
    const snackColor = ref("");
    const snackText = ref("");
    const formDialog = ref(false);
    const confirmationDialog = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "Product name", value: "productName" },
      { text: "Price", value: "price" },
      { text: "Actions", value: "actions", sortable: false },
    ]);
    const editedIndex = ref(-1);
    const formTitle = computed(() =>
      isNewProduct.value ? "New Product" : "Edit Product"
    );

    const openSnack = (txt: string, color: string) => {
      snack.value = true;
      snackColor.value = color;
      snackText.value = txt;
    };

    const edit = (product: IProduct) => {
      isNewProduct.value = false;
      editedIndex.value = products.value.indexOf(product);
      editedProduct.value = product;
      formDialog.value = true;
    };

    const remove = (product: IProduct) => {
      isNewProduct.value = false;
      editedIndex.value = products.value.indexOf(product);
      editedProduct.value = product;
      confirmationDialog.value = true;
    };

    const closeFormDialog = () => {
      formDialog.value = false;
      nextTick(() => {
        editedProduct.value = defaultProduct.value;
        editedIndex.value = -1;
      });
    };

    const confirmationDialogCancel = () => {
      confirmationDialog.value = false;
      nextTick(() => {
        editedProduct.value = defaultProduct.value;
        editedIndex.value = -1;
      });
    };

    const confirmationDialogOk = async () => {
      try {
        const response = await removeById(editedProduct.value.id);
        if (response.ok) {
          openSnack("Product deleted", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }

      confirmationDialogCancel();
    };

    const save = async () => {
      if (
        editedProduct.value.productName.length < 1 ||
        editedProduct.value.price < 1
      ) {
        openSnack("productName or price cannot be empty", "warning");
        return;
      }

      if (editedIndex.value > -1) {
        await updateProduct();
      } else {
        await createProduct();
      }

      closeFormDialog();
    };

    const updateProduct = async () => {
      try {
        const payload: IUpdateProduct = {
          productName: editedProduct.value.productName,
          price: editedProduct.value.price,
        };
        const response = await updateById(editedProduct.value.id, payload);

        if (response.ok) {
          openSnack("Product updated", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }
    };

    const createProduct = async () => {
      try {
        const payload: ICreateProduct = {
          productName: editedProduct.value.productName,
          price: editedProduct.value.price,
        };
        const response = await create(payload);
        if (response.ok) {
          openSnack("Product created", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }
    };

    watch(formDialog, (val) => {
      val || closeFormDialog();
    });

    watch(confirmationDialog, (val) => {
      val || confirmationDialogCancel();
    });

    onBeforeMount(
      async () => await getAll().catch((error) => openSnack(error, "error"))
    );

    return {
      isNewProduct,
      snack,
      snackText,
      snackColor,
      formDialog,
      confirmationDialog,
      search,
      headers,
      products,
      editedProduct,
      formTitle,
      edit,
      remove,
      confirmationDialogOk,
      closeFormDialog,
      confirmationDialogCancel,
      save,
    };
  },
});
</script>
