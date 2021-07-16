import { IProduct } from "./product";

export interface IOrderProduct extends IProduct {
  quantity: number;
}
