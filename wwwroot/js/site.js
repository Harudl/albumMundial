// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {

    // Escuchar todos los clics del documento
    document.addEventListener("click", async function (e) {
        // Buscar si el elemento clickeado (o su padre) tiene el atributo data-ajax="true"
        const link = e.target.closest('a[data-ajax="true"]');

        if (link) {
            e.preventDefault(); // 🚫 Detiene la recarga de página normal

            const url = link.getAttribute("href");
            const contenedor = document.getElementById("main-content");

            try {
                // Hacemos la petición AJAX usando Fetch
                const response = await fetch(url, {
                    headers: {
                        "X-Requested-With": "XMLHttpRequest" // Cabecera para que el controlador lo detecte
                    }
                });

                if (response.ok) {
                    const html = await response.text();

                    // Inyectamos el nuevo HTML sin recargar
                    contenedor.innerHTML = html;

                    // Actualizamos la URL en el navegador por si el usuario recarga manualmente
                    history.pushState(null, "", url);

                    // 🔄 ¡SÚPER IMPORTANTE! Re-inicializar los iconos de Lucide en la nueva vista
                    if (typeof lucide !== 'undefined') {
                        lucide.createIcons();
                    }

                    // Opcional: Cambiar la clase 'active-page' en el menú
                    document.querySelectorAll(".custom-nav-link").forEach(nav => nav.classList.remove("active-page"));
                    link.classList.add("active-page");

                } else {
                    console.error("Error al cargar la página");
                }
            } catch (error) {
                console.error("Error de red:", error);
            }
        }
    });

    // Permitir que los botones "Atrás" y "Adelante" del navegador funcionen correctamente
    window.addEventListener("popstate", function () {
        window.location.reload();
    });
});