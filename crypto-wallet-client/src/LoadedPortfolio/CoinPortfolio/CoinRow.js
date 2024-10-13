import React from 'react'

const CoinRow = ({ coin }) => {
    return (
        <tr className="coinTableRow">
            <td>{coin.name}</td>
            <td>{coin.amount}</td>
            <td>{coin.buyPrice}</td>
        </tr>
    )
}

export default CoinRow
