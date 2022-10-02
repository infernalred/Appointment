<template>
  <button><slot></slot></button>
</template>

<script>
export default {
  name: "SlotButton",
};
</script>

<style scoped>
button,
a {
  height: 32px;
  color: rgb(59, 179, 189);
  font-size: 14px;
  -webkit-box-align: center;
  align-items: center;
  -webkit-box-pack: center;
  justify-content: center;
  border: 1px solid transparent;
  cursor: pointer;
  font-weight: 500;
  border-radius: 2px;
  background-color: rgb(243, 250, 251);
}

a:hover,
a:active,
button:hover,
button:active {
  border-color: rgb(59, 179, 189);
}
</style>
