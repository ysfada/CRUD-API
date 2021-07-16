export interface IOrder {
  id: number;
  customerId: number;
  productId: number;
  quantity: number;
}

export type ICreateOrder = Pick<
  IOrder,
  "customerId" | "productId" | "quantity"
>;
