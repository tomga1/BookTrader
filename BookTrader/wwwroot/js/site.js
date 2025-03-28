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
        if (urlInput.value.trim() !== "") {
            fileInput.value = ""; // Borra el archivo si hay URL
            fileInput.disabled = true;
            previewImg.src = urlInput.value;
            previewImg.style.display = "block";
        } else {
            fileInput.disabled = false;
        }
    } else if (tipo === "archivo") {
        if (fileInput.files.length > 0) {
            urlInput.value = ""; // Borra la URL si hay archivo
            urlInput.disabled = true;

            // Cargar previsualización de imagen
            let reader = new FileReader();
            reader.onload = function (e) {
                previewImg.src = e.target.result;
                previewImg.style.display = "block";
            };
            reader.readAsDataURL(fileInput.files[0]);
        } else {
            urlInput.disabled = false;
        }
    }
}



