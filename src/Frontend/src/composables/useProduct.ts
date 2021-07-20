import { readonly, ref } from "@vue/composition-api";
import { ICreateProduct, IProduct, IUpdateProduct } from "@/@types/product";
import useState from "./useState";

export default function useProduct() {
  const { baseURL } = useState();

  const products = ref<IProduct[]>([]);
  const defaultProduct = ref<IProduct>({
    id: 0,
    productName: "",
    price: 0,
  });
  const editedProduct = ref<IProduct>({
    id: 0,
    productName: "",
    price: 0,
  });

  async function getAll() {
    const response = await fetch(`${baseURL.value}/product`);
    const clone = response.clone();
    if (clone.ok) {
      products.value = await clone.json();
    }
    return response;
  }

  async function removeById(id: number) {
    const response = await fetch(`${baseURL.value}/product/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
    });

    if (response.ok) {
      products.value.splice(
        products.value.findIndex((product) => product.id === id),
        1
      );
    }

    return response;
  }

  async function updateById(id: number, product: IUpdateProduct) {
    const response = await fetch(`${baseURL.value}/product/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(product),
    });
    if (response.ok) {
      Object.assign(
        products.value[
          products.value.findIndex((product) => product.id === id)
        ],
        editedProduct.value
      );
    }
    return response;
  }

  async function create(product: ICreateProduct) {
    const response = await fetch(`${baseURL.value}/product`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(product),
    });

    if (response.ok) {
      products.value.push(await response.json());
    }
    return response;
  }

  return {
    products: readonly(products),
    defaultProduct,
    editedProduct,
    getAll,
    removeById,
    updateById,
    create,
  };
}
