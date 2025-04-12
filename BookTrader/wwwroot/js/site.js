function confirmarEliminacion(id) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡No podrás revertir esto!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Si el usuario confirma, enviamos la solicitud para eliminar
            window.location.href = '/Categorias/Delete/' + id;
        }
    });
}



function validarImagen(tipo) {
    let urlInput = document.getElementById("imagenUrl");
    let fileInput = document.getElementById("imagenArchivo");
    let previewImg = document.getElementById("previewImg");

    if (tipo === "url") {
        // Solo ejecuta si el input de URL existe
        if (urlInput) {
            if (urlInput.value.trim() !== "") {
                if (fileInput) {
                    fileInput.value = ""; // Borra el archivo si hay URL
                    fileInput.disabled = true;
                }
                previewImg.src = urlInput.value;
                previewImg.style.display = "block";
            } else {
                if (fileInput) {
                    fileInput.disabled = false;
                }
            }
        }
    }
    if (tipo === "archivo") {
        if (fileInput.files.length > 0) {
            // Si existe el input de URL, vaciarlo y deshabilitarlo
            if (urlInput) {
                urlInput.value = "";
                urlInput.disabled = true;
            }
            // Cargar previsualización de imagen usando FileReader
            let reader = new FileReader();
            reader.onload = function (e) {
                previewImg.src = e.target.result;
                previewImg.style.display = "block";
            };
            reader.readAsDataURL(fileInput.files[0]);
        } else {
            if (urlInput) {
                urlInput.disabled = false;
            }
        }
    }
}


function cargarSubcategorias(categoriaId) {
    $.ajax({
        url: '/SubCategorias/Lista',  // Asegúrate de que esta ruta sea correcta
        data: { categoriaId: categoriaId },
        type: 'GET',
        success: function (data) {
            $('#subcategoria').html(data);
        },
        error: function () {
            alert('Ocurrió un error al cargar las subcategorías.');
        }
    });
}




