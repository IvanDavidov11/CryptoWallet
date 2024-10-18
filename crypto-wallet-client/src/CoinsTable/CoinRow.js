import React from 'react'

const CoinRow = ({ coin }) => {
    const valueClass = coin.percentageChange?.toString().includes('-') ? 'value-negative' : 'value-positive';

    return (
        <tr className="coinTableRow">
            <td>{coin.id}</td>
            <td>{coin.name}</td>
            <td>{coin.amount}</td>
            <td>${coin.buyPrice}</td>
            <td>${coin.currentPrice}</td>
            <td className={valueClass} style={{textAlign:'right', paddingRight:'70px'}}>{coin.percentageChange}</td>
        </tr>
    )
}

export default CoinRow