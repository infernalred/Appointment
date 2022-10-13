<template>
  <n-form ref="formRef" :model="model" :rules="rules">
    <n-form-item path="email" label="Почта">
      <n-input
        v-model:value.trim="model.email"
        placeholder="Введите почту"
      ></n-input>
    </n-form-item>
    <n-form-item path="password" label="Пароль">
      <n-input
        type="password"
        v-model:value.trim="model.password"
        placeholder="Введите пароль"
      ></n-input>
    </n-form-item>
    <n-button fluid type="primary" @click="submit">Login</n-button>
  </n-form>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { FormInst, FormItemRule, FormValidationError } from "naive-ui";
import LoginModel from "@/models/LoginModel";

export default defineComponent({
  name: "LoginForm",
  emit: ["save-data"],
  setup(_, { emit }) {
    const formRef = ref<FormInst | null>(null);
    const modelRef = ref<LoginModel>({
      email: null,
      password: null,
    });
    return {
      formRef,
      model: modelRef,
      rules: {
        email: [
          {
            required: true,
            trigger: ["blur", "input"],
            validator(rule: FormItemRule, value: string) {
              if (!value) {
                return new Error("Введите почту");
              } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                return new Error("Почтовый адрес не валидный");
              }
              return true;
            },
          },
        ],
        password: {
          required: true,
          trigger: ["blur", "input"],
          message: "Введите пароль",
        },
      },
      submit(e: MouseEvent) {
        e.preventDefault();
        formRef.value?.validate(
          (errors: Array<FormValidationError> | undefined) => {
            if (!errors) {
              emit("save-data", modelRef.value);
            }
          }
        );
      },
    };
  },
});
</script>

<style scoped>
button {
  width: 100%;
}
</style>
