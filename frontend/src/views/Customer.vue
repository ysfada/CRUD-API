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
          <v-toolbar-title>Customer</v-toolbar-title>
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
                New Customer
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
                        v-model="editedCustomer.id"
                        readonly
                        label="Id"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedCustomer.firstName"
                        label="First name"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedCustomer.lastName"
                        label="Last name"
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
                <v-btn color="blue darken-1" text @click="deleteCustomerConfirm"
                  >OK</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon small class="mr-2" @click="editCustomer(item)">
          mdi-pencil
        </v-icon>
        <v-icon small @click="deleteCustomer(item)"> mdi-delete </v-icon>
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
import useCustomer from "@/composables/useCustomer";
import {
  computed,
  defineComponent,
  nextTick,
  onBeforeMount,
  ref,
  watch,
} from "@vue/composition-api";
import {
  ICreateCustomer,
  ICustomer,
  IUpdateCustomer,
} from "../@types/customer";

export default defineComponent({
  setup() {
    const {
      getAllCustomers,
      deleteCustomerById,
      updateCustomer,
      createCustomer,
    } = useCustomer();
    const snack = ref(false);
    const snackColor = ref("");
    const snackText = ref("");
    const dialog = ref(false);
    const dialogDelete = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "First name", value: "firstName" },
      { text: "Last name", value: "lastName" },
      { text: "Actions", value: "actions", sortable: false },
    ]);
    const customers = ref<ICustomer[]>([]);
    const editedIndex = ref(-1);
    const editedCustomer = ref<ICustomer>({
      id: 0,
      firstName: "",
      lastName: "",
    });
    const defaultCustomer = ref<ICustomer>({
      id: 0,
      firstName: "",
      lastName: "",
    });

    const formTitle = computed(() =>
      editedIndex.value === -1 ? "New Customer" : "Edit Customer"
    );

    const editCustomer = (customer: ICustomer) => {
      editedIndex.value = customers.value.indexOf(customer);
      editedCustomer.value = Object.assign({}, customer);
      dialog.value = true;
    };

    const deleteCustomer = (customer: ICustomer) => {
      editedIndex.value = customers.value.indexOf(customer);
      editedCustomer.value = Object.assign({}, customer);
      dialogDelete.value = true;
    };

    const deleteCustomerConfirm = async () => {
      try {
        const response = await deleteCustomerById(editedCustomer.value.id);
        if (response.ok) {
          customers.value.splice(editedIndex.value, 1);
          showSnack("Customer deleted", "info");
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
        editedCustomer.value = Object.assign({}, defaultCustomer.value);
        editedIndex.value = -1;
      });
    };

    const closeDelete = () => {
      dialogDelete.value = false;
      nextTick(() => {
        editedCustomer.value = Object.assign({}, defaultCustomer.value);
        editedIndex.value = -1;
      });
    };

    const save = async () => {
      // TODO validate data
      if (
        editedCustomer.value.firstName.length < 1 ||
        editedCustomer.value.lastName.length < 1
      )
        return;

      if (editedIndex.value > -1) {
        try {
          const body: IUpdateCustomer = {
            firstName: editedCustomer.value.firstName,
            lastName: editedCustomer.value.lastName,
          };
          const response = await updateCustomer(editedCustomer.value.id, body);

          if (response.ok) {
            Object.assign(
              customers.value[editedIndex.value],
              editedCustomer.value
            );
            dialog.value = true;
            showSnack("Customer updated", "info");
          } else {
            // TODO: handle 4xx, 3xx
          }
        } catch (error) {
          showSnack(error, "error");
        }
      } else {
        try {
          const body: ICreateCustomer = {
            firstName: editedCustomer.value.firstName,
            lastName: editedCustomer.value.lastName,
          };
          const response = await createCustomer(body);
          if (response.ok) {
            customers.value.push(await response.json());
            showSnack("Customer created", "info");
          } else {
            // TODO: handle 4xx, 3xx
          }
        } catch (error) {
          showSnack(error, "error");
        }
      }

      close();
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
      customers,
      editedIndex,
      editedCustomer,
      defaultCustomer,
      formTitle,
      editCustomer,
      deleteCustomer,
      deleteCustomerConfirm,
      close,
      closeDelete,
      save,
    };
  },
});
</script>
