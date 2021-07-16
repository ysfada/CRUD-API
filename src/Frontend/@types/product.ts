export interface IProduct {
  id: number;
  productName: string;
  price: number;
}

export type ICreateProduct = Pick<IProduct, "productName" | "price">;
export type IUpdateProduct = Pick<IProduct, "productName" | "price">;
