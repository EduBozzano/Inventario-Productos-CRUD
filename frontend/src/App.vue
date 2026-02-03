<script setup>
import { ref, onMounted } from 'vue';

// --- ESTADO ---
const productos = ref([]);

// Objeto reactivo para el formulario (aquí se guarda lo que escribas en los inputs)
const nuevoProducto = ref({
  nombre: '',
  precio: 0,
  stock: 0
});

// --- FUNCIONES ---

// 1. OBTENER (GET)
const obtenerProductos = async () => {
  try {
    const respuesta = await fetch('http://localhost:5114/api/productos');
    productos.value = await respuesta.json();
  } catch (error) {
    console.error('Error:', error);
  }
};

// 2. CREAR (POST)
const guardarProducto = async () => {
  try {
    // Enviamos los datos al backend
    const respuesta = await fetch('http://localhost:5114/api/productos', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json' // Avisamos que enviamos JSON
      },
      body: JSON.stringify(nuevoProducto.value) // Convertimos el objeto JS a texto JSON
    });

    // Si salió bien...
    if (respuesta.ok) {
      // A. Limpiamos el formulario
      nuevoProducto.value = { nombre: '', precio: 0, stock: 0 };
      // B. Recargamos la lista para ver el nuevo producto
      obtenerProductos(); 
      alert("¡Producto guardado!");
    } else {
      alert("Error al guardar");
    }
  } catch (error) {
    console.error(error);
  }
};

// --- CICLO DE VIDA ---
onMounted(() => {
  obtenerProductos();
});
</script>

<template>
  <div class="contenedor">
    <h1>📦 Gestión de Inventario</h1>

    <div class="formulario">
      <h2>Nuevo Producto</h2>
      <form @submit.prevent="guardarProducto">
        <input v-model="nuevoProducto.nombre" placeholder="Nombre del producto" required />
        <input v-model="nuevoProducto.precio" type="number" placeholder="Precio" step="0.01" required />
        <input v-model="nuevoProducto.stock" type="number" placeholder="Stock" required />
        <button type="submit">Agregar</button>
      </form>
    </div>
    
    <hr>

    <ul class="lista">
      <li v-for="item in productos" :key="item.id" class="tarjeta-producto">
        <div>
          <strong>{{ item.nombre }}</strong>
          <p>Stock: {{ item.stock }} | ID: {{ item.id }}</p>
        </div>
        <div class="precio">
          ${{ item.precio }}
        </div>
      </li>
    </ul>
  </div>
</template>

<style scoped>
.contenedor { max-width: 600px; margin: 0 auto; font-family: Arial, sans-serif; padding: 20px; }
h1 { text-align: center; color: #42b883; }

/* Estilos del Formulario */
.formulario { background: #eee; padding: 15px; border-radius: 8px; margin-bottom: 20px; }
input { padding: 8px; margin-right: 5px; border: 1px solid #ccc; border-radius: 4px; }
button { background: #42b883; color: white; border: none; padding: 8px 15px; border-radius: 4px; cursor: pointer; }
button:hover { background: #3aa876; }

/* Estilos de la Lista */
.lista { list-style: none; padding: 0; }
.tarjeta-producto { background: white; border: 1px solid #ddd; padding: 10px; margin-bottom: 10px; border-radius: 5px; display: flex; justify-content: space-between; align-items: center; box-shadow: 0 2px 4px rgba(0,0,0,0.05); }
.precio { font-weight: bold; color: #35495e; }
</style>