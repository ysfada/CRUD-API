import { readonly, ref } from "@vue/composition-api";
import { ICreateOrder, IOrder, IUpdateOrder } from "@/@types/order";
import { IOrderProduct } from "@/@types/orderProduct";
import useState from "./useState";
import useProduct from "./useProduct";

export default function useOrder() {
  const { baseURL } = useState();
  const { products, getAll: getAllProducts } = useProduct();

  const orders = ref<IOrder[]>([]);
  const orderProducts = ref<IOrderProduct[]>([]);
  const defaultOrder = ref<IOrder>({
    id: 0,
    customerId: 0,
    productId: 0,
    quantity: 0,
  });
  const editedOrder = ref<IOrder>({
    id: 0,
    customerId: 0,
    productId: 0,
    quantity: 0,
  });
  const defaultProduct = ref<IOrderProduct>({
    id: 0,
    productName: "",
    price: 0,
    quantity: 0,
  });
  const editedProduct = ref<IOrderProduct>({
    id: 0,
    productName: "",
    price: 0,
    quantity: 0,
  });

  async function setProducts() {
    await getAllProducts();
    orderProducts.value = products.value.map((product) => ({
      ...product,
      quantity: 0,
    }));
  }

  async function getAll() {
    const response = await fetch(`${baseURL.value}/order`);
    const clone = response.clone();
    if (clone.ok) {
      orders.value = await clone.json();
    }
    return response;
  }

  async function removeById(id: number) {
    const response = await fetch(`${baseURL.value}/order/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
    });

    if (response.ok) {
      orders.value.splice(
        orders.value.findIndex((order) => order.id === id),
        1
      );
    }

    return response;
  }

  async function updateById(id: number, order: IUpdateOrder) {
    const response = await fetch(`${baseURL.value}/order/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(order),
    });
    if (response.ok) {
      Object.assign(
        orders.value[orders.value.findIndex((order) => order.id === id)],
        editedOrder.value
      );
    }
    return response;
  }

  async function create(order: ICreateOrder) {
    const response = await fetch(`${baseURL.value}/order`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(order),
    });

    if (response.ok) {
      orders.value.push(await response.json());
    }
    return response;
  }

  return {
    orders: readonly(orders),
    orderProducts: readonly(orderProducts),
    defaultOrder,
    editedOrder,
    defaultProduct,
    editedProduct,
    getAll,
    removeById,
    updateById,
    create,
    setProducts,
  };
}
