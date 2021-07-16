<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <v-btn exact depressed plain :ripple="false" :to="{ name: 'Home' }">
        <div class="d-flex align-center">
          <v-img
            alt="Vuetify Logo"
            class="shrink mr-2"
            contain
            src="https://cdn.vuetifyjs.com/images/logos/vuetify-logo-dark.png"
            transition="scale-transition"
            width="40"
          />

          <v-img
            alt="Vuetify Name"
            class="shrink mt-1 hidden-sm-and-down"
            contain
            min-width="100"
            src="https://cdn.vuetifyjs.com/images/logos/vuetify-name-dark.png"
            width="100"
          />
        </div>
      </v-btn>

      <v-spacer></v-spacer>

      <v-btn :to="{ name: 'Customer' }" text>
        <span class="mr-2">Customer CRUD</span>
      </v-btn>
      <v-btn :to="{ name: 'Product' }" text>
        <span class="mr-2">Product CRUD</span>
      </v-btn>
      <v-btn :to="{ name: 'Order' }" text>
        <span class="mr-2">Order</span>
      </v-btn>

      <v-spacer></v-spacer>

      <v-btn href="https://github.com/ysfada/CRUD-API" target="_blank" text>
        <span class="mr-2">Github</span>
        <v-icon>mdi-open-in-new</v-icon>
      </v-btn>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { defineComponent } from "@vue/composition-api";

export default defineComponent({
  name: "App",
  setup() {
    return {};
  },
});
</script>
