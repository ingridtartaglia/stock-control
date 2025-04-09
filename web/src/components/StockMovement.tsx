import React, { useState, useEffect } from 'react';
import {
    Box,
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    TextField,
    Typography,
    Alert
} from '@mui/material';
import { stockService, StockMovement, Product } from '../services/api';

export const StockMovementForm: React.FC = () => {
    const [movement, setMovement] = useState<StockMovement>({
        productCode: '',
        type: 'In',
        quantity: 0
    });
    const [products, setProducts] = useState<Product[]>([]);
    const [error, setError] = useState<string>('');
    const [success, setSuccess] = useState<boolean>(false);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const productsList = await stockService.getProducts();
                setProducts(productsList);
            } catch (err: any) {
                setError(err.message || 'Error loading products');
            }
        };

        fetchProducts();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setSuccess(false);

        if (movement.quantity <= 0) {
            setError('Quantity must be greater than zero');
            return;
        }

        if (!movement.productCode) {
            setError('Please select a product');
            return;
        }

        try {
            await stockService.addMovement(movement);
            setSuccess(true);
            setMovement({
                productCode: '',
                type: 'In',
                quantity: 0
            });
        } catch (err: any) {
            setError(err.response?.data?.title || err.response?.data || err.message || 'Error on adding movement');
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ maxWidth: 400, mx: 'auto', mt: 4 }}>
            <Typography variant="h5" component="h2" gutterBottom>
                Stock Movement
            </Typography>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}

            {success && (
                <Alert severity="success" sx={{ mb: 2 }}>
                    Movement added successfully!
                </Alert>
            )}

            <FormControl fullWidth margin="normal" required>
                <InputLabel>Product</InputLabel>
                <Select
                    value={movement.productCode}
                    label="Product"
                    onChange={(e) => setMovement({ ...movement, productCode: e.target.value })}
                >
                    <MenuItem value="">
                        <em>Select a product</em>
                    </MenuItem>
                    {products.map((product) => (
                        <MenuItem key={product.id} value={product.code}>
                            {product.name} ({product.code})
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>

            <FormControl fullWidth margin="normal">
                <InputLabel>Type</InputLabel>
                <Select
                    value={movement.type}
                    label="Type"
                    onChange={(e) => setMovement({ ...movement, type: e.target.value as 'In' | 'Out' })}
                >
                    <MenuItem value="In">In</MenuItem>
                    <MenuItem value="Out">Out</MenuItem>
                </Select>
            </FormControl>

            <TextField
                fullWidth
                label="Quantity"
                type="number"
                value={movement.quantity}
                onChange={(e) => {
                    const value = parseInt(e.target.value);
                    if (value > 0) {
                        setMovement({ ...movement, quantity: value });
                    }
                }}
                margin="normal"
                required
                inputProps={{ min: 1 }}
            />

            <Button
                type="submit"
                variant="contained"
                color="primary"
                fullWidth
                sx={{ mt: 2 }}
            >
                Add Movement
            </Button>
        </Box>
    );
};