<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="customers"
      :search="search"
      sort-by="id"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Customers</v-toolbar-title>
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
                @click="isNewCustomer = true"
              >
                New Customer
              </v-btn>
            </template>
            <v-form ref="formRef" v-model="valid" lazy-validation>
              <v-card>
                <v-card-title>
                  <span class="text-h5">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col v-show="!isNewCustomer" cols="12" sm="6">
                        <v-text-field
                          v-model="editedCustomer.id"
                          readonly
                          required
                          label="Id"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="6">
                        <v-text-field
                          v-model="editedCustomer.firstName"
                          label="First name"
                          :rules="rules"
                          required
                        ></v-text-field>
                      </v-col>
                      <v-col cols="12" sm="6">
                        <v-text-field
                          v-model="editedCustomer.lastName"
                          label="Last name"
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
          <v-dialog
            v-model="confirmationDialog"
            max-width="500px"
            @click:outside="confirmationDialogCancel"
          >
            <v-card>
              <v-card-title class="text-h5"
                >Are you sure you want to delete this customer?</v-card-title
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
import { ICreateCustomer, ICustomer, IUpdateCustomer } from "@/@types/customer";
import useCustomer from "@/composables/useCustomer";

export default defineComponent({
  name: "Customer",
  setup() {
    const {
      customers,
      defaultCustomer,
      editedCustomer,
      getAll,
      removeById,
      updateById,
      create,
    } = useCustomer();

    const formRef = ref<HTMLFormElement>();
    const valid = ref(true);
    const isNewCustomer = ref(true);
    const snack = ref(false);
    const snackColor = ref("");
    const snackText = ref("");
    const formDialog = ref(false);
    const confirmationDialog = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "First name", value: "firstName" },
      { text: "Last name", value: "lastName" },
      { text: "Actions", value: "actions", sortable: false },
    ]);
    const editedIndex = ref(-1);
    const formTitle = computed(() =>
      isNewCustomer.value ? "New Customer" : "Edit Customer"
    );

    const rules = ref([
      (value: string) => !!value || "Required.",
      (value: string) => (value || "").length <= 30 || "Max 30 characters",
    ]);

    const openSnack = (txt: string, color: string) => {
      snack.value = true;
      snackColor.value = color;
      snackText.value = txt;
    };

    const edit = (customer: ICustomer) => {
      isNewCustomer.value = false;
      editedIndex.value = customers.value.indexOf(customer);
      editedCustomer.value = customer;
      formDialog.value = true;
    };

    const remove = (customer: ICustomer) => {
      isNewCustomer.value = false;
      editedIndex.value = customers.value.indexOf(customer);
      editedCustomer.value = customer;
      confirmationDialog.value = true;
    };

    const closeFormDialog = async () => {
      await getAll().catch((error) => openSnack(error, "error"));
      formDialog.value = false;
      formRef.value?.resetValidation();
      nextTick(() => {
        editedCustomer.value = defaultCustomer.value;
        editedIndex.value = -1;
      });
    };

    const confirmationDialogCancel = () => {
      confirmationDialog.value = false;
      nextTick(() => {
        editedCustomer.value = defaultCustomer.value;
        editedIndex.value = -1;
      });
    };

    const confirmationDialogOk = async () => {
      try {
        const response = await removeById(editedCustomer.value.id);
        if (response.ok) {
          openSnack("Customer deleted", "success");
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
        await updateCustomer();
      } else {
        await createCustomer();
      }

      closeFormDialog();
    };

    const updateCustomer = async () => {
      try {
        const payload: IUpdateCustomer = {
          firstName: editedCustomer.value.firstName,
          lastName: editedCustomer.value.lastName,
        };
        const response = await updateById(editedCustomer.value.id, payload);

        if (response.ok) {
          openSnack("Customer updated", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }
    };

    const createCustomer = async () => {
      try {
        const payload: ICreateCustomer = {
          firstName: editedCustomer.value.firstName,
          lastName: editedCustomer.value.lastName,
        };
        const response = await create(payload);
        if (response.ok) {
          openSnack("Customer created", "success");
        } else {
          openSnack(response.statusText, "warning");
        }
      } catch (error) {
        openSnack(error, "error");
      }
    };

    watch(confirmationDialog, (val) => {
      val || confirmationDialogCancel();
    });

    onBeforeMount(
      async () => await getAll().catch((error) => openSnack(error, "error"))
    );

    return {
      formRef,
      valid,
      isNewCustomer,
      snack,
      snackText,
      snackColor,
      formDialog,
      confirmationDialog,
      search,
      headers,
      customers,
      editedCustomer,
      formTitle,
      rules,
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
