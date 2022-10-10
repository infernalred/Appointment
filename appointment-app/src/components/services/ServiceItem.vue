<template>
  <div>
    <base-card>
      <header>
        <h2>{{ service.title }}</h2>
      </header>
      <img alt="" :src="image" />
      <h4>{{ service.description }}</h4>
      <router-link :to="serviceLink"
        ><n-button
          strong
          secondary
          round
          type="primary"
          size="small"
          class="n-button"
          >Подробнее</n-button
        ></router-link
      >
    </base-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import Service from "@/models/Service";

export default defineComponent({
  name: "ServiceItem",
  props: {
    service: {
      type: Object as PropType<Service>,
      required: true,
    },
  },
  computed: {
    image() {
      return this.service.image
        ? this.service.image
        : require("../../assets/service.png");
    },
    serviceLink() {
      return this.$route.path + "/" + this.service.id;
    },
  },
});
</script>

<style scoped>
img {
  width: 150px;
  height: 150px;
}
a {
  text-decoration: none;
}
</style>
