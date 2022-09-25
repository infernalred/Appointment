import { createApp } from "vue";
import App from "@/App.vue";
import router from "@/router";
import { createPinia } from "pinia";

import BaseCard from "@/components/ui/BaseCard.vue";
import BaseButton from "@/components/ui/BaseButton.vue";

const pinia = createPinia();
const app = createApp(App);

app.use(pinia);
app.use(router);

app.component("base-card", BaseCard);
app.component("base-button", BaseButton);

app.mount("#app");
