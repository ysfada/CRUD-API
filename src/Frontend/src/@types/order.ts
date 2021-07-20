import { ICustomer } from "./customer";
import { IProduct } from "./product";

export interface IOrder {
  id: number;
  customerId: number;
  productId: number;
  quantity: number;

  customer?: ICustomer;
  product?: IProduct;
}

export type ICreateOrder = Pick<
  IOrder,
  "customerId" | "productId" | "quantity"
>;

export type IUpdateOrder = Pick<
  IOrder,
  "customerId" | "productId" | "quantity"
>;
