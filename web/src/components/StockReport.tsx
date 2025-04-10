import React, { useState } from 'react';
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
    Alert,
    Button
} from '@mui/material';
import { stockService, StockReport } from '../services/api';
import { useTranslation } from 'react-i18next';

export const StockReportView: React.FC = () => {
    const { t } = useTranslation();
    const [date, setDate] = useState<string>(new Date().toISOString().split('T')[0]);
    const [productCode, setProductCode] = useState<string>('');
    const [report, setReport] = useState<StockReport[]>([]);
    const [error, setError] = useState<string>('');
    const [hasSearched, setHasSearched] = useState<boolean>(false);

    const fetchReport = async () => {
        try {
            const data = await stockService.getStockReport(date, productCode || undefined);
            setReport(data);
            setError('');
            setHasSearched(true);
        } catch (err: any) {
            setError(t('stock.report.errors.loading'));
            setReport([]);
            setHasSearched(true);
        }
    };

    return (
        <Box sx={{ maxWidth: 800, mx: 'auto', mt: 4 }}>
            <Typography variant="h5" component="h2" gutterBottom>
                {t('stock.report.title')}
            </Typography>

            <Box sx={{ display: 'flex', gap: 2, mb: 3 }}>
                <TextField
                    label={t('stock.report.date')}
                    type="date"
                    value={date}
                    onChange={(e) => setDate(e.target.value)}
                    InputLabelProps={{ shrink: true }}
                />
                <TextField
                    label={t('stock.report.product-code')}
                    value={productCode}
                    onChange={(e) => setProductCode(e.target.value)}
                />
                <Button
                    variant="contained"
                    onClick={fetchReport}
                    sx={{ minWidth: 120 }}
                >
                    {t('stock.report.search')}
                </Button>
            </Box>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}

            {hasSearched && (
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>{t('stock.report.table.product')}</TableCell>
                                <TableCell>{t('stock.report.table.code')}</TableCell>
                                <TableCell align="right">{t('stock.report.table.in')}</TableCell>
                                <TableCell align="right">{t('stock.report.table.out')}</TableCell>
                                <TableCell align="right">{t('stock.report.table.balance')}</TableCell>
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
            )}
        </Box>
    );
};
