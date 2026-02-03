<script setup>
// 1. IMPORTAR HERRAMIENTAS
import { ref, onMounted } from 'vue';

// 2. ESTADO (Variables Reactivas)
// Usamos 'ref' para que Vue sepa que si esto cambia, debe actualizar la pantalla.
const productos = ref([]);

// 3. FUNCIONES (Lógica)
const obtenerProductos = async () => {
  try {
    // Hacemos la petición a la API de .NET
    const respuesta = await fetch('http://localhost:5114/api/productos');
    
    // Convertimos la respuesta de texto a JSON (Array de objetos)
    const datos = await respuesta.json();
    
    // Guardamos los datos en nuestra variable reactiva
    productos.value = datos; 
  } catch (error) {
    console.error('Error al obtener productos:', error);
  }
};

// 4. CICLO DE VIDA
// onMounted se ejecuta automáticamente cuando la página termina de cargar
onMounted(() => {
  obtenerProductos();
});
</script>

<template>
  <div class="contenedor">
    <h1>📦 Gestión de Inventario</h1>
    
    <ul>
      <li v-for="item in productos" :key="item.id" class="tarjeta-producto">
        <div>
          <strong>{{ item.nombre }}</strong>
          <p>Stock: {{ item.stock }} unidades</p>
        </div>
        <div class="precio">
          ${{ item.precio }}
        </div>
      </li>
    </ul>

    <p v-if="productos.length === 0">Cargando productos...</p>
  </div>
</template>

<style scoped>
/* CSS básico */
.contenedor {
  max-width: 600px;
  margin: 0 auto;
  font-family: Arial, sans-serif;
  padding: 20px;
}

h1 {
  text-align: center;
  color: #42b883; /* Verde Vue */
}

ul {
  list-style: none;
  padding: 0;
}

.tarjeta-producto {
  background: #f9f9f9;
  border: 1px solid #ddd;
  padding: 15px;
  margin-bottom: 10px;
  border-radius: 8px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.precio {
  font-weight: bold;
  font-size: 1.2em;
  color: #35495e;
}
</style>