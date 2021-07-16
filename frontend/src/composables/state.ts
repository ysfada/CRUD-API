import { ref } from "@vue/composition-api";

export default function useState() {
  const baseURL = ref("http://localhost:53495/api");
  return { baseURL };
}
