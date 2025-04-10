import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5250/api'
});

// Add response interceptor for error handling
api.interceptors.response.use(
    response => response,
    error => {
        if (error.response) {
            // The request was made and the server responded with a status code
            // that falls out of the range of 2xx
            const errorMessage = error.response.data?.title || error.response.data?.detail || error.response.data || 'An error occurred';
            return Promise.reject(new Error(errorMessage));
        } else if (error.request) {
            // The request was made but no response was received
            return Promise.reject(new Error('No response received from server'));
        } else {
            // Something happened in setting up the request that triggered an Error
            return Promise.reject(new Error('Error setting up the request'));
        }
    }
);

export interface Product {
    id: string;
    name: string;
    code: string;
}

export interface StockMovement {
    productCode: string;
    type: 'In' | 'Out';
    quantity: number;
}

export interface StockReport {
    productName: string;
    productCode: string;
    totalIn: number;
    totalOut: number;
    balance: number;
}

export const stockService = {
    addMovement: async (movement: StockMovement) => {
        const response = await api.post<StockMovement>('/stock/movements', movement);
        return response.data;
    },
    getStockReport: async (date: string, productCode?: string) => {
        // Ensure date is in YYYY-MM-DD format
        const formattedDate = new Date(date).toISOString().split('T')[0];
        const response = await api.get<StockReport[]>('/stock/reports', {
            params: { 
                date: formattedDate,
                productCode: productCode || undefined 
            }
        });
        return response.data;
    },
    getProducts: async () => {
        const response = await api.get<Product[]>('/products');
        return response.data;
    }
};