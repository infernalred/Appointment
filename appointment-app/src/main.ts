import { createApp } from "vue";
import App from "@/App.vue";
import router from "@/router";
import { createPinia } from "pinia";
import Toast, { PluginOptions } from "vue-toastification";
import { create, NButton, NInput, NForm, NFormItem, NResult } from "naive-ui";
import "vue-toastification/dist/index.css";

import BaseCard from "@/components/ui/BaseCard.vue";
import BaseButton from "@/components/ui/BaseButton.vue";

const pinia = createPinia();
const app = createApp(App);

const options: PluginOptions = {
  transition: "Vue-Toastification__bounce",
  maxToasts: 20,
  newestOnTop: true,
};

const naive = create({
  components: [NButton, NInput, NForm, NFormItem, NResult],
});

app.use(pinia);
app.use(router);
app.use(naive);
app.use(Toast, options);

app.component("base-card", BaseCard);
app.component("base-button", BaseButton);

app.mount("#app");
