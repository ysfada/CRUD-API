<template>
  <Info />
</template>

<script lang="ts">
import { defineComponent } from "@vue/composition-api";
import Info from "../components/Info.vue";

export default defineComponent({
  name: "Home",
  components: {
    Info,
  },
});
</script>
