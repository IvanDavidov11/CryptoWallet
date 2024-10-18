import React from 'react'

const CoinRow = ({ coin, index }) => {
    const valueClass = coin.percentageChange?.toString().includes('-') ? 'value-negative' : 'value-positive';

    const formattedBuyPrice = coin.buyPrice.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 6 });
    const formattedCurrentPrice = coin.currentPrice.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 6 });

    return (
        <tr className="coinTableRow">
            <td>{index + 1}</td>
            <td>{coin.name}</td>
            <td>{coin.amount}</td>
            <td>${formattedBuyPrice}</td>
            <td>${formattedCurrentPrice}</td>
            <td className={valueClass} style={{textAlign:'right', paddingRight:'70px'}}>{coin.percentageChange}</td>
        </tr>
    )
}

export default CoinRow