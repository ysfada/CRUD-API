import { ref } from "@vue/composition-api";

export default function useState() {
  const baseURL = ref("http://localhost:5000/api");
  return { baseURL };
}
