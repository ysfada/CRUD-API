import { readonly, ref } from "@vue/composition-api";
import { ICreateCustomer, ICustomer, IUpdateCustomer } from "@/@types/customer";
import useState from "./useState";

export default function useCustomer() {
  const { baseURL } = useState();

  const customers = ref<ICustomer[]>([]);
  const defaultCustomer = ref<ICustomer>({
    id: 0,
    firstName: "",
    lastName: "",
  });
  const editedCustomer = ref<ICustomer>({
    id: 0,
    firstName: "",
    lastName: "",
  });

  async function getAll() {
    const response = await fetch(`${baseURL.value}/customer`);
    const clone = response.clone();
    if (clone.ok) {
      customers.value = await clone.json();
    }
    return response;
  }

  async function removeById(id: number) {
    const response = await fetch(`${baseURL.value}/customer/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
    });

    if (response.ok) {
      customers.value.splice(
        customers.value.findIndex((customer) => customer.id === id),
        1
      );
    }

    return response;
  }

  async function updateById(id: number, customer: IUpdateCustomer) {
    const response = await fetch(`${baseURL.value}/customer/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(customer),
    });
    if (response.ok) {
      Object.assign(
        customers.value[
          customers.value.findIndex((customer) => customer.id === id)
        ],
        editedCustomer.value
      );
    }
    return response;
  }

  async function create(customer: ICreateCustomer) {
    const response = await fetch(`${baseURL.value}/customer`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(customer),
    });

    if (response.ok) {
      customers.value.push(await response.json());
    }
    return response;
  }

  return {
    customers: readonly(customers),
    defaultCustomer,
    editedCustomer,
    getAll,
    removeById,
    updateById,
    create,
  };
}
