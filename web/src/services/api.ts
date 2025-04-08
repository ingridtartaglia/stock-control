import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5250/api'
});

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
        const response = await api.post<StockMovement>('/StockMovement/movement', movement);
        return response.data;
    },
    getStockReport: async (date: string, productCode?: string) => {
        const response = await api.get<StockReport[]>('/StockMovement/report', {
            params: { date, productCode }
        });
        return response.data;
    }
};