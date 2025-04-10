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
import { useTranslation } from 'react-i18next';

export const StockMovementForm: React.FC = () => {
    const { t } = useTranslation();
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
                setError(t('stock.movement.errors.loading-products'));
            }
        };

        fetchProducts();
    }, [t]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setSuccess(false);

        if (movement.quantity <= 0) {
            setError(t('stock.movement.errors.quantity-zero'));
            return;
        }

        if (!movement.productCode) {
            setError(t('stock.movement.errors.select-product'));
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
            const errorMessage = err.response?.data?.title || err.response?.data || err.message;
            if (errorMessage?.toLowerCase().includes('insufficient quantity')) {
                setError(t('stock.movement.errors.insufficient-quantity'));
            } else {
                setError(errorMessage || t('stock.movement.errors.add-movement'));
            }
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ maxWidth: 400, mx: 'auto', mt: 4 }}>
            <Typography variant="h5" component="h2" gutterBottom>
                {t('stock.movement.title')}
            </Typography>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}

            {success && (
                <Alert severity="success" sx={{ mb: 2 }}>
                    {t('stock.movement.add-success')}
                </Alert>
            )}

            <FormControl fullWidth margin="normal" required>
                <InputLabel>{t('stock.movement.product')}</InputLabel>
                <Select
                    value={movement.productCode}
                    label={t('stock.movement.product')}
                    onChange={(e) => setMovement({ ...movement, productCode: e.target.value })}
                >
                    <MenuItem value="">
                        <em>{t('stock.movement.select-product')}</em>
                    </MenuItem>
                    {products.map((product) => (
                        <MenuItem key={product.id} value={product.code}>
                            {product.name} ({product.code})
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>

            <FormControl fullWidth margin="normal">
                <InputLabel>{t('stock.movement.type')}</InputLabel>
                <Select
                    value={movement.type}
                    label={t('stock.movement.type')}
                    onChange={(e) => setMovement({ ...movement, type: e.target.value as 'In' | 'Out' })}
                >
                    <MenuItem value="In">{t('stock.movement.in')}</MenuItem>
                    <MenuItem value="Out">{t('stock.movement.out')}</MenuItem>
                </Select>
            </FormControl>

            <TextField
                fullWidth
                margin="normal"
                required
                type="number"
                value={movement.quantity}
                label={t('stock.movement.quantity')}
                onChange={(e) => {
                    const value = parseInt(e.target.value);
                    if (value > 0) {
                        setMovement({ ...movement, quantity: value });
                    }
                }}
                inputProps={{ min: 1 }}
            />

            <Button
                type="submit"
                variant="contained"
                color="primary"
                fullWidth
                sx={{ mt: 2 }}
            >
                {t('stock.movement.save')}
            </Button>
        </Box>
    );
};