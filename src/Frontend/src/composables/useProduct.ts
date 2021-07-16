import { IUpdateProduct } from "../@types/product";
import useState from "./state";

const { baseURL } = useState();

function getAllProducts() {
  return fetch(`${baseURL.value}/Product`);
}

function deleteProductById(id: number) {
  return fetch(`${baseURL.value}/Product/${id}`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
  });
}

function updateProduct(id: number, product: IUpdateProduct) {
  return fetch(`${baseURL.value}/Product/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(product),
  });
}

function createProduct(product: IUpdateProduct) {
  return fetch(`${baseURL.value}/Product`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(product),
  });
}

export default function useProduct() {
  return { getAllProducts, deleteProductById, updateProduct, createProduct };
}
