import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", title: "User | Home", data: { alias: "user-home" }, redirectTo:'/financas', pathMatch:'full' },
];
