import React, { useState, useEffect } from 'react';
import {
    Box,
    TextField,
    Typography,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    Alert
} from '@mui/material';
import { stockService, StockReport } from '../services/api';

export const StockReportView: React.FC = () => {
    const [date, setDate] = useState<string>(new Date().toISOString().split('T')[0]);
    const [productCode, setProductCode] = useState<string>('');
    const [report, setReport] = useState<StockReport[]>([]);
    const [error, setError] = useState<string>('');

    const fetchReport = async () => {
        try {
            const data = await stockService.getStockReport(date, productCode || undefined);
            setReport(data);
            setError('');
        } catch (err: any) {
            setError(err.response?.data || 'Erro ao carregar relatório');
            setReport([]);
        }
    };

    useEffect(() => {
        fetchReport();
    }, [date, productCode]);

    return (
        <Box sx={{ maxWidth: 800, mx: 'auto', mt: 4 }}>
            <Typography variant="h5" component="h2" gutterBottom>
                Relatório de Estoque
            </Typography>

            <Box sx={{ display: 'flex', gap: 2, mb: 3 }}>
                <TextField
                    label="Data"
                    type="date"
                    value={date}
                    onChange={(e) => setDate(e.target.value)}
                    InputLabelProps={{ shrink: true }}
                />
                <TextField
                    label="Código do Produto"
                    value={productCode}
                    onChange={(e) => setProductCode(e.target.value)}
                />
            </Box>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Produto</TableCell>
                            <TableCell>Código</TableCell>
                            <TableCell align="right">Entradas</TableCell>
                            <TableCell align="right">Saídas</TableCell>
                            <TableCell align="right">Saldo</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {report.map((row) => (
                            <TableRow key={row.productCode}>
                                <TableCell>{row.productName}</TableCell>
                                <TableCell>{row.productCode}</TableCell>
                                <TableCell align="right">{row.totalIn}</TableCell>
                                <TableCell align="right">{row.totalOut}</TableCell>
                                <TableCell align="right">{row.balance}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
};
