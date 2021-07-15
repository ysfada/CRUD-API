<template>
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
                      v-model="editedCustomer.first_name"
                      label="First name"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      v-model="editedCustomer.last_name"
                      label="Last name"
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
import { ICustomer } from "../@types/customer";

export default defineComponent({
  setup() {
    const dialog = ref(false);
    const dialogDelete = ref(false);
    const search = ref("");
    const headers = ref([
      { text: "Id", align: "start", value: "id" },
      { text: "First name", value: "first_name" },
      { text: "Last name", value: "last_name" },
      { text: "Actions", value: "actions", sortable: false },
    ]);
    const customers = ref<ICustomer[]>([]);
    const editedIndex = ref(-1);
    const editedCustomer = ref<ICustomer>({
      id: 0,
      first_name: "",
      last_name: "",
    });
    const defaultCustomer = ref<ICustomer>({
      id: 0,
      first_name: "",
      last_name: "",
    });

    const formTitle = computed(() =>
      editedIndex.value === -1 ? "New Customer" : "Edit Customer"
    );

    const mock = () => {
      customers.value = [
        {
          id: 1,
          first_name: "first name 1",
          last_name: "last name 1",
        },
        {
          id: 2,
          first_name: "first name 2",
          last_name: "last name 2",
        },
        {
          id: 3,
          first_name: "first name 3",
          last_name: "last name 3",
        },
        {
          id: 4,
          first_name: "first name 4",
          last_name: "last name 4",
        },
        {
          id: 5,
          first_name: "first name 5",
          last_name: "last name 5",
        },
        {
          id: 6,
          first_name: "first name 6",
          last_name: "last name 6",
        },
        {
          id: 7,
          first_name: "first name 7",
          last_name: "last name 7",
        },
        {
          id: 8,
          first_name: "first name 8",
          last_name: "last name 8",
        },
        {
          id: 9,
          first_name: "first name 9",
          last_name: "last name 9",
        },
        {
          id: 10,
          first_name: "first name 10",
          last_name: "last name 10",
        },
      ];
    };

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

    const deleteCustomerConfirm = () => {
      customers.value.splice(editedIndex.value, 1);
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

    const save = () => {
      if (editedIndex.value > -1) {
        Object.assign(customers.value[editedIndex.value], editedCustomer.value);
      } else {
        customers.value.push(editedCustomer.value);
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
      customers,
      editedIndex,
      editedCustomer,
      defaultCustomer,
      formTitle,
      mock,
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
