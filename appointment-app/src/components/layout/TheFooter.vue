<template>
  <footer>
    <nav>
      <li><a href="https://github.com/infernalred">Created by</a></li>
      <div>
        <h4>&copy; All rights reserved - {{ new Date().getFullYear() }}</h4>
      </div>
    </nav>
  </footer>
</template>

<script>
export default {
  name: "TheFooter",
};
</script>

<style scoped>
footer {
  width: 100%;
  height: 5rem;
  background-color: #3d008d;
  display: flex;
  justify-content: center;
  align-items: center;
  bottom: 0;
  position: sticky;
}

footer a {
  text-decoration: none;
  color: #f391e3;
  display: inline-block;
  padding: 0.75rem 1.5rem;
  border: 1px solid transparent;
}

a:active,
a:hover,
a.router-link-active {
  border: 1px solid #f391e3;
}

h4 {
  color: white;
  margin: 0;
}

h1 {
  color: white;
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

footer nav {
  width: 20%;
  margin: auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

footer ul {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

li {
  margin: 0 0.5rem;
}
</style>
