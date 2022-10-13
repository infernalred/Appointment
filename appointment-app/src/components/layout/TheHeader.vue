<template>
  <header>
    <nav>
      <li><router-link to="/">Услуги</router-link></li>
      <ul>
        <li><router-link to="/services">Услуги</router-link></li>
        <li><router-link to="/masters">Мастера</router-link></li>
        <li>
          <router-link v-if="isAuthenticated" to="/dashboard"
            >Управление</router-link
          ><router-link v-else to="/auth">Войти</router-link>
        </li>
      </ul>
    </nav>
  </header>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { useAppointmentStore } from "@/store";

export default defineComponent({
  name: "TheHeader",
  setup() {
    const store = useAppointmentStore();
    const showModalRef = ref(false);
    return {
      model: showModalRef,
      isAuthenticated: computed(() => store.isAuthenticated),
    };
  },
});
</script>

<style scoped>
header {
  width: 100%;
  height: 5rem;
  background-color: #3bb3bd;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-left: auto;
}

header a {
  text-decoration: none;
  color: #fff;
  display: inline-block;
  padding: 0.75rem 1.5rem;
  border: 1px solid transparent;
}

a:active,
a:hover,
a.router-link-active {
  border: 1px solid #1f6885;
}

h1 {
  margin: 0;
}

h1 a {
  color: white;
  margin: 0;
}

h1 a:hover,
h1 a:active,
h1 a.router-link-active {
  border-color: transparent;
}

header nav {
  max-width: 40rem;
  margin: auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

header ul {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

li {
  margin: 0 0.5rem;
  list-style: none;
}
</style>
