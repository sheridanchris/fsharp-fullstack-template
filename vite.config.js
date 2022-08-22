const { defineConfig } = require("vite");
const react = require("@vitejs/plugin-react");

// https://vitejs.dev/config/
module.exports = defineConfig({
    root: "src/Client",
    build: {
        outDir: "../../deploy/public/",
        emptyOutDir: true,
        sourcemap: true
    },
    server: {
        proxy: {
            '/api': {
                target: 'http://localhost:8888',
                changeOrigin: true,
            }
        }
    },
    plugins: [react()],
});