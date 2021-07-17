import { readonly, ref } from "@vue/composition-api";
import { ICreateOrder } from "@/@types/order";
import { IOrderProduct } from "@/@types/orderProduct";
import useState from "./state";
import useProduct from "./useProduct";

export default function useOrder() {
  const { baseURL } = useState();
  const { products, getAll: getAllProducts } = useProduct();

  const orderProducts = ref<IOrderProduct[]>([]);
  const orderedProduct = ref<IOrderProduct>({
    id: 0,
    productName: "",
    price: 0,
    quantity: 0,
  });
  const defaultProduct = ref<IOrderProduct>({
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

  function createOrder(order: ICreateOrder) {
    return fetch(`${baseURL.value}/Order`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(order),
    });
  }

  return {
    orderProducts: readonly(orderProducts),
    defaultProduct,
    orderedProduct,
    setProducts,
    createOrder,
  };
}
