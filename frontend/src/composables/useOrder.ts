import useState from "./state";
import { ICreateOrder } from "../@types/order";

const { baseURL } = useState();

function createOrder(order: ICreateOrder) {
  return fetch(`${baseURL.value}/Order`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(order),
  });
}

export default function useOrder() {
  return { createOrder };
}
