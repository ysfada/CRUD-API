import { ICreateCustomer, IUpdateCustomer } from "../@types/customer";
import { ICreateOrder } from "../@types/order";
import useState from "./state";

const { baseURL } = useState();

function getAllCustomers() {
  return fetch(`${baseURL.value}/Customer`);
}

function deleteCustomerById(id: number) {
  return fetch(`${baseURL.value}/Customer/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
  });
}

function updateCustomer(id: number, customer: IUpdateCustomer) {
  return fetch(`${baseURL.value}/Customer/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(customer),
  });
}

function createCustomer(customer: ICreateCustomer) {
  return fetch(`${baseURL.value}/Customer`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(customer),
  });
}

export default function useCustomer() {
  return {
    getAllCustomers,
    deleteCustomerById,
    updateCustomer,
    createCustomer,
  };
}
