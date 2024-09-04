/// <reference types="vite/client" />
interface ImportMetaEnv {
    readonly VITE_API_URL: string
    readonly VITE_API_KEY: string
    readonly VITE_BASE_URL: string;
    // другие переменные окружения...
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}