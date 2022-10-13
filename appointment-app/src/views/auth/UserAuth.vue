<template>
  <section>
    <base-card>
      <login-form @save-data="login"></login-form>
    </base-card>
  </section>
</template>

<script lang="ts">
import LoginForm from "@/components/authentification/LoginForm.vue";
import { useAppointmentStore } from "@/store";
import LoginModel from "@/models/LoginModel";
import { defineComponent } from "vue";
import router from "@/router";
import { useToast } from "vue-toastification";

const toast = useToast();
export default defineComponent({
  name: "UserAuth",
  components: { LoginForm },
  data() {
    return {
      store: useAppointmentStore(),
    };
  },
  methods: {
    async login(cred: LoginModel) {
      try {
        await this.store.login(cred);
        await router.replace("/dashboard");
      } catch (e) {
        console.log(e);
        toast.error("Неправильные логин или пароль");
      }
    },
  },
});
</script>

<style scoped></style>
