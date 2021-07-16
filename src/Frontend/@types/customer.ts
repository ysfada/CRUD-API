export interface ICustomer {
  id: number;
  firstName: string;
  lastName: string;
}

export type ICreateCustomer = Pick<ICustomer, "firstName" | "lastName">;
export type IUpdateCustomer = Pick<ICustomer, "firstName" | "lastName">;
