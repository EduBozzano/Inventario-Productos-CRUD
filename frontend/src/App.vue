<script setup>
import { ref, onMounted, computed } from 'vue';

// --- ESTADO ---
const productos = ref([]);
const modoEdicion = ref(false); // Estamos editando o creando?
const nuevoProducto = ref({
  id: 0,
  nombre: '',
  precio: null,
  stock: null,
  fechaCreacion: new Date() // Para que no vaya vacío
});

// --- FUNCIONES CRUD ---

// 1. LEER (GET)
const obtenerProductos = async () => {
  try {
    const respuesta = await fetch('http://localhost:5114/api/productos');
    productos.value = await respuesta.json();
  } catch (error) {
    console.error('Error:', error);
  }
};

// 2. GUARDAR (Maneja CREAR y ACTUALIZAR)
const guardarProducto = async () => {
  try {
    let metodo = 'POST';
    let url = 'http://localhost:5114/api/productos';

    // Si estamos en modo edición, cambiamos a PUT y ajustamos la URL
    if (modoEdicion.value) {
      metodo = 'PUT';
      url = `http://localhost:5114/api/productos/${nuevoProducto.value.id}`;
    }

    const respuesta = await fetch(url, {
      method: metodo,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(nuevoProducto.value)
    });

    if (respuesta.ok) {
      limpiarFormulario();
      obtenerProductos(); // Recargar lista
    } else {
      alert("Error al guardar");
    }
  } catch (error) {
    console.error(error);
  }
};

// 3. ELIMINAR (DELETE)
const eliminarProducto = async (id) => {
  if(!confirm("¿Seguro que deseas eliminar este producto?")) return;

  try {
    const respuesta = await fetch(`http://localhost:5114/api/productos/${id}`, {
      method: 'DELETE'
    });

    if (respuesta.ok) {
      obtenerProductos(); // Recargar lista
    } else {
      alert("Error al eliminar");
    }
  } catch (error) {
    console.error(error);
  }
};

// --- FUNCIONES AUXILIARES ---

// Cargar datos en el formulario para editar
const iniciarEdicion = (producto) => {
  // Usamos {...producto} para crear una copia y no modificar la lista directamente mientras escribimos
  nuevoProducto.value = { ...producto };
  modoEdicion.value = true;
};

const limpiarFormulario = () => {
  nuevoProducto.value = { id: 0, nombre: '', precio: null, stock: null };
  modoEdicion.value = false;
};

const esFormularioValido = computed(() => {
  if (!nuevoProducto.value.nombre || !nuevoProducto.value.stock || !nuevoProducto.value.precio) {
    return false;
  }
  if (nuevoProducto.value.stock <= 0 || nuevoProducto.value.precio <= 0) {
    return false
  }
  return true;
});

// --- CICLO DE VIDA ---
onMounted(() => {
  obtenerProductos();
});
</script>

<template>
  <div class="contenedor">
    <h1>📦 Gestión de Inventario</h1>

    <div class="formulario" :class="{ 'modo-edicion': modoEdicion }">
      <h2>{{ modoEdicion ? '✏️ Editar Producto' : '✨ Nuevo Producto' }}</h2>
      
      <form @submit.prevent="guardarProducto">
        <input v-model="nuevoProducto.nombre" placeholder="Nombre" required />
        <input v-model="nuevoProducto.precio" type="number" placeholder="Precio" step="0.01" required />
        <input v-model="nuevoProducto.stock" type="number" placeholder="Stock" required />
        
        <button type="submit" :class="[modoEdicion ? 'btn-editar' : 'btn-agregar' , {'gris-opaco': !esFormularioValido}]" :disabled="!esFormularioValido">
          {{ modoEdicion ? 'Actualizar' : 'Agregar' }}
        </button>
        
        <button v-if="modoEdicion" type="button" @click="limpiarFormulario" class="btn-cancelar">
          Cancelar
        </button>
      </form>
    </div>
    
    <hr>

    <ul class="lista">
      <li v-for="item in productos" :key="item.id" class="tarjeta-producto">
        <div class="info">
          <strong>{{ item.nombre }}</strong>
          <span class="stock" :class="{ 'alerta-roja': item.stock < 10}">Stock: {{ item.stock }}</span>
        </div>
        
        <div class="acciones">
          <span class="precio">${{ item.precio }}</span>
          
          <button @click="iniciarEdicion(item)" class="btn-small btn-blue">✏️</button>
          <button @click="eliminarProducto(item.id)" class="btn-small btn-red">🗑️</button>
        </div>
      </li>
    </ul>
  </div>
</template>

<style scoped>
.contenedor { max-width: 600px; margin: 0 auto; font-family: 'Segoe UI', sans-serif; padding: 20px; }
h1 { text-align: center; color: #42b883; }

/* Formulario */
.formulario { background: #f4f4f4; padding: 20px; border-radius: 8px; margin-bottom: 20px; border-left: 5px solid #42b883; transition: all 0.3s; }
.formulario.modo-edicion { border-left-color: #3498db; background: #eaf2f8; }
input { padding: 10px; margin: 5px; border: 1px solid #ddd; border-radius: 4px; width: 28%; }

/* Botones */
button { border: none; padding: 10px 15px; border-radius: 4px; cursor: pointer; font-weight: bold; margin-left: 5px; }
.btn-agregar { background: #42b883; color: white; }
.btn-editar { background: #3498db; color: white; }
.btn-cancelar { background: #95a5a6; color: white; }
.btn-small { padding: 5px 10px; font-size: 1.2em; }
.btn-blue { background: #e1f5fe; color: #0288d1; }
.btn-red { background: #ffebee; color: #d32f2f; }
button:hover { opacity: 0.9; transform: scale(1.05); }

/* Lista */
.lista { list-style: none; padding: 0; }
.tarjeta-producto { background: white; border-bottom: 1px solid #eee; padding: 15px; display: flex; justify-content: space-between; align-items: center; }
.info { display: flex; flex-direction: column; }
.stock { font-size: 0.8em; color: #777; }
.precio { font-weight: bold; font-size: 1.2em; color: #2c3e50; margin-right: 15px; }
.acciones { display: flex; align-items: center; }

.alerta-roja{
  color: RED;
  font-weight: bold;
}

.gris-opaco{
  background: gray;
  cursor: not-allowed;
}
</style>