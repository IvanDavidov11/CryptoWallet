import React from 'react'

const CoinRow = ({ coin, hasApiCalculations }) => {
    return (
        <tr className="coinTableRow">
            <td>{coin.name}</td>
            <td>{coin.amount}</td>
            <td>{coin.buyPrice}</td>
            
            {hasApiCalculations &&
                <td>{coin.percentageChange}</td>
            }
        </tr>
    )
}

export default CoinRow
