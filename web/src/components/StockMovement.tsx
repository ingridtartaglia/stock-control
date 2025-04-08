import React, { useState } from 'react';
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
import { stockService, StockMovement } from '../services/api';

export const StockMovementForm: React.FC = () => {
    const [movement, setMovement] = useState<StockMovement>({
        productCode: '',
        type: 'In',
        quantity: 0
    });
    const [error, setError] = useState<string>('');
    const [success, setSuccess] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setSuccess(false);

        try {
            await stockService.addMovement(movement);
            setSuccess(true);
            setMovement({
                productCode: '',
                type: 'In',
                quantity: 0
            });
        } catch (err: any) {
            setError(err.response?.data || 'Erro ao adicionar movimentação');
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ maxWidth: 400, mx: 'auto', mt: 4 }}>
            <Typography variant="h5" component="h2" gutterBottom>
                Movimentação de Estoque
            </Typography>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}

            {success && (
                <Alert severity="success" sx={{ mb: 2 }}>
                    Movimentação adicionada com sucesso!
                </Alert>
            )}

            <TextField
                fullWidth
                label="Código do Produto"
                value={movement.productCode}
                onChange={(e) => setMovement({ ...movement, productCode: e.target.value })}
                margin="normal"
                required
            />

            <FormControl fullWidth margin="normal">
                <InputLabel>Tipo</InputLabel>
                <Select
                    value={movement.type}
                    label="Tipo"
                    onChange={(e) => setMovement({ ...movement, type: e.target.value as 'In' | 'Out' })}
                >
                    <MenuItem value="In">Entrada</MenuItem>
                    <MenuItem value="Out">Saída</MenuItem>
                </Select>
            </FormControl>

            <TextField
                fullWidth
                label="Quantidade"
                type="number"
                value={movement.quantity}
                onChange={(e) => setMovement({ ...movement, quantity: parseInt(e.target.value) })}
                margin="normal"
                required
            />

            <Button
                type="submit"
                variant="contained"
                color="primary"
                fullWidth
                sx={{ mt: 2 }}
            >
                Adicionar Movimentação
            </Button>
        </Box>
    );
};