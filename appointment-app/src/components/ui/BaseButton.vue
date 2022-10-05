<template>
  <button v-if="!link" :class="mode">
    <slot></slot>
  </button>
  <router-link v-else :to="to" :class="mode">
    <slot></slot>
  </router-link>
</template>

<script>
export default {
  props: {
    mode: {
      type: String,
      required: false,
      default: null,
    },
    link: {
      type: Boolean,
      required: false,
      default: false,
    },
    to: {
      type: String,
      required: false,
      default: "/",
    },
  },
};
</script>

<style scoped>
button,
a {
  text-decoration: none;
  padding: 0.75rem 1.5rem;
  font: inherit;
  background-color: #3bb3bd;
  border: 1px solid #3bb3bd;
  color: white;
  cursor: pointer;
  border-radius: 30px;
  margin-right: 0.5rem;
  display: inline-block;
}

a:hover,
a:active,
button:hover,
button:active {
  background-color: #1f6885;
  border-color: #1f6885;
}

button:disabled {
  background-color: rgba(19, 1, 1, 0.3);
  border-color: rgba(195, 195, 195, 0.3);
  color: rgba(16, 16, 16, 0.3);
}

.flat {
  background-color: transparent;
  color: #3bb3bd;
  border: none;
}

.outline {
  background-color: transparent;
  border-color: #1f6885;
  color: #1f6885;
}

.flat:hover,
.flat:active,
.outline:hover,
.outline:active {
  background-color: #1f6885;
}
</style>
